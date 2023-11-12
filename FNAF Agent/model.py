import torch
import torch.nn as nn
import torch.optim as optim
import torch.nn.functional as F
import os

class Linear_QNet(nn.Module): #creates a linear model with qnet that contains 11 , 256 , 3 (INTRO AND LAST STAY THE SAME!!)
    def __init__(self):
        super().__init__()
        self.linear1 = nn.Linear(19, 256)
        self.linear2 = nn.Linear(256, 16)

    def forward(self,x):
        print(f"{x}")
        x = F.relu(self.linear1(x))
        x = self.linear2(x)
        return x
    
    def save(self, file_name='model.pth'):
        model_folder_path = "./model"
        if not os.path.exists(model_folder_path):
            os.mkdir(model_folder_path)

        file_name = os.path.join(model_folder_path, file_name)
        torch.save(self.state_dict(), file_name)

class QTrainer:
    def __init__(self, model, lr, gamma):
        self.lr = lr
        self.gamma = gamma
        self.model = model

        self.optimizer = optim.Adam(model.parameters(), lr=self.lr)
        self.criterion = nn.MSELoss()#lose function

    def train_step(self, state, action, reward, next_state, done):
        state = torch.tensor(state, dtype = torch.float)
        next_state = torch.tensor(next_state, dtype = torch.float)
        action = torch.tensor(action, dtype = torch.float)
        reward = torch.tensor(reward, dtype = torch.float)
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
