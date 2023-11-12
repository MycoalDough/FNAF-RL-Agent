import torch
import random
import numpy as np
from collections import deque
import data
from model import Linear_QNet, QTrainer
import time
import threading
from helper import plot

#from helper import plot
#agent learning curve
#[power, doorLeft, doorRight, bonniePos, bonnieTimer, freddyPos, freddyTimer, foxyPos, foxyTimer, chicaPos, chicaTimer, freddySFX]

#the doors determine whether they're close or open
#the pos's are their current position 
#timer's are how long has it been since you last checked on them
#freddySFX is the sound he makes when moving

MAX_MEMORY = 100_000
BATCH_SIZE = 1000
LR = 0.001

class Agent:
    def __init__(self): #create the agent (need number of games, epsilon value (80 or 90 idk yet))
        self.num_games = 0
        self.epsilon = 0 #randomness control
        self.gamma = 0.9 #discount rate 
        self.memory = deque(maxlen=MAX_MEMORY) #popleft() called after deque becomes full
        self.model = Linear_QNet()
        self.trainer = QTrainer(self.model, lr=LR, gamma = self.gamma)

    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done)) #popleft() if MAX_MEMORY is reached

    def train_long_memory(self):
        if(len(self.memory) > BATCH_SIZE):
            mini_sample = random.sample(self.memory, BATCH_SIZE)
        else:
            mini_sample = self.memory

        states, actions, rewards, next_states, dones = zip(*mini_sample)
        self.trainer.train_step(states, actions, rewards, next_states, dones)

    def train_short_memory(self, state, action, reward, next_state, done):
        self.trainer.train_step(state, action, reward, next_state, done)


    def get_action(self, state):
        #random moves: tradeoff between exploration and exploitation
        self.epsilon = 80 - self.num_games
        if random.randint(0, 200) < self.epsilon:
            move = random.randint(0,16)
        else:
            state0 = torch.tensor(state, dtype=torch.float)
            prediction = self.model(state0)
            move = torch.argmax(prediction).item()
        return move
    
def train():
    plot_scores = []
    plot_mean_scores = []
    agent = Agent()
    #data.create_host()

    def on_connection_established(client_socket):
        total_score = 0
        record = 0
        print("Connection established")
        while True:
            time.sleep(.3)
            #get old state
            state_old = data.get_state()
            #get move
            final_move = agent.get_action(state_old)
            #preform move and get new state
            reward, done, score = data.play_step(final_move)
            
            state_new = data.get_state()
            #train short memory (1 step)
            agent.train_short_memory(state_old, final_move, reward, state_new, done)
            #remember
            agent.remember(state_old, final_move, reward, state_new, done)
            if(done):
                #train long memory and plot result
                data.reset()
                agent.num_games+=1
                agent.train_long_memory()

                if score > record:
                    record = score
                    agent.model.save() #when model is created
                
                print("Game", agent.num_games, 'Score', score, "Record:", record)
                #plot matplotlib when you install it bud stfu 
                #1:46 am 11/4/23
                #stfu
                #9:51pm 11/10/23 
                #L
                plot_scores.append(score)
                total_score += score
                mean_score = total_score / agent.num_games
                plot_mean_scores.append(mean_score)
                plot(plot_scores, plot_mean_scores)
    data.create_host(on_connection_established)



if __name__ == "__main__":
    train()