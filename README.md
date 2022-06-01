# FinancialRiskSimulator

Financial Risk Simulator using Monte Carlo Approach for financial assets.

## Prerequisites

You will need the following tools:

* [Visual Studio Code or Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (version 17.0.0 Preview 7.0 or later)
* [JetBrains Rider ](https://www.jetbrains.com/rider/)
* [.NET Core SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)



## Project Guide

1. Download or Clone this project
```bash
git clone https://github.com/brcdmr/FinancialRiskSimulator.git
```
2. Build and Run

Build and run Docker containers run Docker compose located in the solution directory

```bash
docker-compose -f 'docker-compose.yml' up --build
 
```



## API

#### /Simulation/Submit
* `POST` : You can Submit and Run Simulation, 

 Sample Post Body
```
{
    "T": 120,
    "S": 1000,
    "assets": [
        {
            "name": "Asset 1",
            "p0": 190.0,
            "m": "0.5%",
            "s": "10%"
        },
        {
            "name": "Asset 2",
            "p0": 100.0,
            "m": "0.2%",
            "s": "1%"
        },
        {
            "name": "Asset 3",
            "p0": 75.25,
            "m": "0.4%",
            "s": "8%"
        }
    ]
}
```

TaskId returns to query the simulation status.


#### /Simulation/Query
* `POST` : You can check the simulation state

Sample Post Body
```
{
   "TaskId": "5eeeb439-9769-494f-9f5f-02ceb9171588"
}
```

#### /Simulation/FinalResult
* `POST` : You can get the final simulation results

Sample Post Body
```
{
   "TaskId": "5eeeb439-9769-494f-9f5f-02ceb9171588"
}
```
