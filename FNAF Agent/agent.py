import numpy as np
from model import Agent
from utils import plotLearning
import data
import time
from helper import plot


if(__name__ == "__main__"):
    load_checkpoint = False

    agent = Agent(gamma=0.99,epsilon=1,lr=4e-4, input_dims=[21],n_actions=16, mem_size=20_000, eps_min=0.01, batch_size=64,eps_dec=3e-6,replace=100)

    if load_checkpoint:
        agent.load_models()

    filename = 'FNAF-DDDQN-Adam-lr5e4-replace100.png'
    scores, eps_history = [],[]
    num_games = 10000

    plot_scores = []
    plot_mean_scores = []
    total_score = 0
    record = 0
    cur_games = 0
    best_game = 0
    def on_connection_established(client_socket):
        global total_score, record, plot_mean_scores, plot_scores, cur_games, best_game
        print("Connection established")

        for i in range(num_games):
            done = False
            

            while not done:
                observation = data.get_state()
                action, was_random = agent.choose_action(observation)
                reward, done, speed, score = data.play_step(action)
                observation_ = data.get_state()
                #print(was_random, ": ", reward)
                agent.store_transition(observation, action, reward, observation_, int(done))
                agent.learn()
                time.sleep(.3 / speed) 
                
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
            print('episode', i, 'score %.lf' % score, 'average score %.lf ' % mean_score, 'epsilon %.5f ' % agent.epsilon, 'record %.lf ' % record, ' at episode ', best_game)
            if i > 10 and i % 10 == 0:
                agent.save_models()

            eps_history.append(agent.epsilon)

        print("- training process over -")
        print("- creating model graph -")
        x = [i+1 for i in range(num_games)]
        plotLearning(x,scores,eps_history,filename)

            

    data.create_host(on_connection_established)
