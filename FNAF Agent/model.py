import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F
import os
import numpy as np

class QNetwork(nn.Module): #creates a linear model with qnet that contains 11 , 256 , 3 (INTRO AND LAST STAY THE SAME!!)
    def __init__(self):
        super().__init__()
        self.linear1 = nn.Linear(19, 256)
        self.linear2 = nn.Linear(256, 256)
        self.linear3 = nn.Linear(256, 16)


    def forward(self,x):
        x = F.relu(self.linear1(x))
        x = F.relu(self.linear2(x))
        return self.linear3(x) 
    
    def save(self, file_name='model.pth'):
        model_folder_path = "./model"
        if not os.path.exists(model_folder_path):
            os.mkdir(model_folder_path)

        file_name = os.path.join(model_folder_path, file_name)
        torch.save({'state_dict': self.state_dict(),}, file_name)
        
    def load(self, file_name='model.pth'):
        file_name = os.path.join("./model", file_name)

        # Load the model parameters, record, and epsilon
        checkpoint = torch.load(file_name)
        self.load_state_dict(checkpoint['state_dict'])
        print(f"Successfully loaded!")
        
class QTrainer:
    def __init__(self, model, target, lr, gamma):
        self.lr = lr
        self.gamma = gamma
        self.q_network = model
        self.target_q_network = target
        self.optimizer = optim.Adam(model.parameters(), lr=self.lr)
        self.criterion = nn.MSELoss()#lose function

    def update_target_network(self):
        self.target_q_network.load_state_dict(self.q_network.state_dict())

    def train_step(self, state, action, reward, next_state, done):
        state = torch.tensor(np.array(state), dtype=torch.float)
        next_state = torch.tensor(np.array(next_state), dtype=torch.float)
        action = torch.tensor(np.array(action), dtype=torch.float)
        reward = torch.tensor(np.array(reward), dtype=torch.float)
        done = torch.tensor(np.array(done), dtype=torch.float)
        # (n , x ) where n is the batch size

        if(len(state.shape) == 1):
            # one x lol (1,x) 1 is the batch size
            state = torch.unsqueeze(state, 0)
            next_state = torch.unsqueeze(next_state, 0)
            action = torch.unsqueeze(action, 0)
            reward = torch.unsqueeze(reward, 0)
            done = torch.tensor([done], dtype=torch.float32)  # Convert to tensor
        
        q_values = self.q_network(state)
        next_q_values_target = self.target_q_network(next_state)

        action = action.long()
        target_q_values = next_q_values_target.clone().detach()
        target_q_values[torch.arange(len(action)), action.squeeze().long()] = reward + self.gamma * torch.max(next_q_values_target, dim=1).values * (1 - done)
        loss = self.criterion(q_values, target_q_values)
        self.optimizer.zero_grad()
        loss.backward()
        self.optimizer.step()

        #1) get predicted q values with current state of game
        #
        #pred = self.model(state)
        #target = pred.clone()
        #for i in range(len(done)):
        #    Q_new = reward[i]
        #if( not done[i]):
        #   Q_new = reward[i] + self.gamma * torch.max(self.model(next_state[i]))
        
        #target[i][torch.argmax(action[i]).item()] = Q_new.item()
        #2) q_new = formula (bellman) r + gamma * max(next prediction of q value)
        #clone prediction 
        #preds[argmax(action)] = q_new
        #self.optimizer.zero_grad()
        #loss = self.criterion(target, pred)
        #loss.backward()
        #self.optimizer.step()
