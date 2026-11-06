# Cloud-Ops-project

> **DISCLAIMER:** THE APPLICATION IS STILL UNDER CONSTRUCTION. README WILL BE CHANGED AS NEW FEATURES ARE INTRODUCED.

## Purpose

Ever find the steam recommendation system a little too... broad? You're not alone! Thousands of gamers often wish they could explore games beyondd the general interest matches and top scoring games.

That is why game spotter is here. This application helps you find exactly the game you want based on your unique gaming profile and preferences. You can configure your preferences by liking/disliking games, adding to favorites, importing your steam libary and more!

## How to set it up?

The setup (for now) is extremely easy. Just follow these steps and have the application up and running within around 1-5 minutes.

### Prerequirements

- Docker desktop (Windows/mac)

### Step 1. Clone the repository

Make sure docker desktop is running in the background. This will ensure the docker commands are accessible.

Choose any folder on your computer, make sure it's empty.

Next; open up powershell (or any other terminal) in that folder and run the following command:

```bash
git clone https://github.com/Jamey848/Cloud-Ops
```

### Step 2. Docker compose

In your folder, you should have a copy of the complete repository.

Now navigate to the "Project" folder and open a new terminal within that folder.

Within the terminal run the following command:

```bash
docker-compose up --build -d
```

### Step 3. Access the application

Check the terminal output: you should see 2 containers running. To access the application, visit [http://localhost:8081/](http://localhost:8081/)

Now you should see the home page of the application!