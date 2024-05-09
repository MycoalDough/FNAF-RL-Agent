import numpy as np
from model import Agent
import data
import time
import model as Model
from helper import plot
from utils import plotLearning

if(__name__ == "__main__"):
    load_checkpoint = False


    agent1 = Model.Agent(gamma=0.99,epsilon=1,lr=4e-4, input_dims=[19], n_actions=16, mem_size=300_000, eps_min=0.01, batch_size=32, checkpoint_name="fnaf_model",eps_dec=2e-6,replace=100)


    if load_checkpoint:
        agent1.load_models()


    filename = 'FNAF-1.png'
    scores, eps_history = [],[]
    plot_scores = []
    plot_mean_scores = []
    total_score = 0
    record = 0
    cur_games = 0
    best_game = 0
    num_games = 10000


    def on_connection_established(client_socket):
        global total_score, record, plot_mean_scores, plot_scores, cur_games, best_game
        print("Connection established")


        for i in range(num_games):
            done = False
            speed = 0.1
        

            while not(done):
                observation = data.get_state()
                action, was_random = agent1.choose_action(observation)
                reward, done, speed, score = data.play_step(action)
                print("Agent: ", was_random, ": ", reward, ": ", agent1.epsilon)
                observation_ = data.get_state()
                agent1.store_transition(observation, action, reward, observation_, int(done))
                agent1.learn()
                time.sleep(0.5 / speed)

            cur_games += 1
            data.reset()
            plot_scores.append(score)
            total_score += score
            mean_score = total_score / cur_games

            if record < score:
                record = score
                best_game = i
            plot_mean_scores.append(mean_score)
            plot(plot_scores, plot_mean_scores)

            print('episode', i, 'epsilon %.2f ' % agent1.epsilon)
            if i > 10 and i % 10 == 0:
                agent1.save_models()




        print("- training process over -")
        print("- creating model graph -")
        x = [i+1 for i in range(num_games)]
        plotLearning(x,scores,eps_history,filename)


           


    data.create_host(on_connection_established)