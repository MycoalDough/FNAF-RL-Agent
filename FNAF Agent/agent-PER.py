import torch
import random
import numpy as np
from collections import deque
import data
from model import QNetwork, QTrainer
import time
import threading
import os
from helper import plot
from epsilongreedypolicy import EpsilonGreedyPolicy
from replaybuffer import ReplayBuffer

#from helper import plot
#agent learning curve
#[power, doorLeft, doorRight, bonniePos, bonnieTimer, freddyPos, freddyTimer, foxyPos, foxyTimer, chicaPos, chicaTimer, freddySFX]

#the doors determine whether they're close or open
#the pos's are their current position 
#timer's are how long has it been since you last checked on them
#freddySFX is the sound he makes when moving

MAX_MEMORY = 1_000_000
BATCH_SIZE = 32
LR = 0.0105

class Agent:
    def __init__(self): #create the agent (need number of games, epsilon value (80 or 90 idk yet))
        self.num_games = 0
        self.epsilon = 0 #randomness control
        self.gamma = 0.99 #discount rate 
        self.memory = ReplayBuffer(MAX_MEMORY) #popleft() called after deque becomes full
        self.q_network = QNetwork()
        self.target_q_network = QNetwork()
        self.trainer = QTrainer(self.q_network, self.target_q_network, lr=LR, gamma = self.gamma)

    def remember(self, state, action, reward, next_state, done):
        self.memory.add((state, action, reward, next_state, done)) #popleft() if MAX_MEMORY is reached

    def train_long_memory(self, cur_epsilon):
        mini_sample, importance, indices = self.memory.sample(BATCH_SIZE)
        states, actions, rewards, next_states, dones = zip(*mini_sample)

        errors = self.trainer.train_step(states, actions, rewards, next_states, dones, priorities=importance**(1-cur_epsilon))
        #states_array = np.array(states)
        #td_errors = np.abs(self.trainer.q_network(torch.tensor(states_array, dtype=torch.float32))[np.arange(BATCH_SIZE), torch.tensor(actions, dtype=torch.long)] - torch.tensor(target_q_values, dtype=torch.float32))        
        self.memory.set_priorities(indices, errors)

    def train_short_memory(self, state, action, reward, next_state, done):
        self.trainer.train_step(state, action, reward, next_state, done, np.ones(1))
        print((state, action, reward, next_state, done))
        self.memory.add((state, action, reward, next_state, done))

    def get_action(self, state, epsilon):
        epsilon.decay_epsilon()
        state_tensor = torch.tensor(state, dtype=torch.float32).view(1,-1)

        if epsilon.choose_action(0, 16):
            return np.random.randint(0, 16)
        else:
            advantages = self.q_network.advantage(state_tensor).detach().numpy()
            action = np.argmax(advantages)  # Choose the action with the highest Q-value
            return action

    
def train():
    plot_scores = []
    plot_mean_scores = []
    agent = Agent()
    epsilon = EpsilonGreedyPolicy(initial_epsilon=1, min_epsilon=0.01, decay_rate=0.99999932)
    # Load the pre-trained model
    target_path = r"C:\Users\mibbd\OneDrive\Desktop\FNAF\FNAF Agent\model\target_model.pth"
    model_path = r"C:\Users\mibbd\OneDrive\Desktop\FNAF\FNAF Agent\model\model.pth"
    if os.path.exists(model_path) and os.path.exists(target_path):
        agent.target_q_network.load(target_path)
        agent.q_network.load(model_path)

    def on_connection_established(client_socket):
        total_score = 0
        record = 0
        print("Connection established")

        while True:
            time.sleep(.3 / 5)
            # get old state
            state_old = data.get_state()
            # get move
            final_move = agent.get_action(state_old, epsilon)
            # perform move and get new state
            reward, done, score = data.play_step(final_move)

            state_new = data.get_state()
            # train short memory (1 step)
            agent.train_short_memory(state_old, final_move, reward, state_new, done)
            # remember
            agent.remember(state_old, final_move, reward, state_new, done)
            if done:
                # train long memory and plot result
                data.reset()
                agent.num_games += 1
                agent.train_long_memory(epsilon.epsilon)
                agent.q_network.save("model.pth")  # Save the model with updated record

                if(agent.num_games % 10 == 0):
                    agent.trainer.update_target_network()

                if score > record:
                    record = score
                    agent.target_q_network.save("target_model.pth")  # Save the model with updated record

                print("Game", agent.num_games, 'Score', score, "Record:", record, "Epsilon:", epsilon.epsilon)
                plot_scores.append(score)
                total_score += score
                mean_score = total_score / agent.num_games
                plot_mean_scores.append(mean_score)
                plot(plot_scores, plot_mean_scores)

    data.create_host(on_connection_established)


if __name__ == "__main__":
    train()