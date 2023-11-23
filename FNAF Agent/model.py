import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F
import os
import numpy as np

class Linear_QNet(nn.Module): #creates a linear model with qnet that contains 11 , 256 , 3 (INTRO AND LAST STAY THE SAME!!)
    def __init__(self):
        super().__init__()
        self.linear1 = nn.Linear(19, 256)
        self.linear2 = nn.Linear(256, 16)
        self.record_score = 0
        self.epsilon = 0

    def forward(self,x):
        x = F.relu(self.linear1(x))
        x = self.linear2(x)
        return x
    
    def save(self, file_name='model.pth'):
        model_folder_path = "./model"
        if not os.path.exists(model_folder_path):
            os.mkdir(model_folder_path)

        file_name = os.path.join(model_folder_path, file_name)
        torch.save({
                    'state_dict': self.state_dict(),
                    'record_score': self.record_score,
                    'epsilon': self.epsilon
                }, file_name)
        
    def load(self, file_name='model.pth'):
        file_name = os.path.join("./model", file_name)

        # Load the model parameters, record, and epsilon
        checkpoint = torch.load(file_name)
        self.load_state_dict(checkpoint['state_dict'])
        self.record_score = checkpoint['record_score']
        self.epsilon = checkpoint['epsilon']
        print(f"Successfully loaded! The model's record is {self.record_score}.")
        
class QTrainer:
    def __init__(self, model, lr, gamma):
        self.lr = lr
        self.gamma = gamma
        self.model = model

        self.optimizer = optim.Adam(model.parameters(), lr=self.lr)
        self.criterion = nn.MSELoss()#lose function

    def train_step(self, state, action, reward, next_state, done):
        state = torch.tensor(np.array(state), dtype=torch.float)  # Convert to NumPy array first
        next_state = torch.tensor(np.array(next_state), dtype=torch.float)  # Convert to NumPy array first
        action = torch.tensor(np.array(action), dtype=torch.float)  # Convert to NumPy array first
        reward = torch.tensor(np.array(reward), dtype=torch.float)  # Convert to NumPy array first
        # (n , x ) where n is the batch size

        if(len(state.shape) == 1):
            # one x lol (1,x) 1 is the batch size
            state = torch.unsqueeze(state, 0)
            next_state = torch.unsqueeze(next_state, 0)
            action = torch.unsqueeze(action, 0)
            reward = torch.unsqueeze(reward, 0)
            done = (done, )

        #1) get predicted q values with current state of game
        pred = self.model(state)
        target = pred.clone()
        for i in range(len(done)):
            Q_new = reward[i]
        if( not done[i]):
           Q_new = reward[i] + self.gamma * torch.max(self.model(next_state[i]))
        
        target[i][torch.argmax(action[i]).item()] = Q_new.item()
        #2) q_new = formula (bellman) r + gamma * max(next prediction of q value)
        #clone prediction 
        #preds[argmax(action)] = q_new
        self.optimizer.zero_grad()
        loss = self.criterion(target, pred)
        loss.backward()
        self.optimizer.step()
