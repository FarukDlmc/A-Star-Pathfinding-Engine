# 🧭 A* (A-Star) Pathfinding Engine & Grid Maze Navigation

A high-performance **C#** console application demonstrating the **A* (A-Star) Search Algorithm** to find the optimal/shortest path through a dynamic grid-based maze populated with obstacles and terrain costs.

---

## 📌 Project Overview

Pathfinding is a fundamental problem in autonomous navigation, robotics, and spatial AI. This project implements the A* search algorithm using custom node structures and priority sorting to guarantee the shortest path between a designated **Start Point** and **Target Destination** while efficiently traversing dynamic obstacles.

### Key Highlights
* **Optimal Path Guarantee:** Combines Dijkstra's algorithm benefits with heuristic-driven greedy best-first search.
* **Heuristic Optimization:** Implements Manhattan / Euclidean distance heuristic functions to minimize search space expansion.
* **Real-time Visualization:** Renders the maze, open/closed sets, evaluated nodes, and the final calculated path directly on the console interface.

---

## ⚙️ Algorithmic Mechanics & Heuristics

The A* engine evaluates candidate path nodes based on the core fitness formula:

$$f(n) = g(n) + h(n)$$

* **$g(n)$:** The exact cost of the path from the starting node to candidate node $n$.
* **$h(n)$:** The estimated heuristic cost from node $n$ to the final goal destination (Manhattan Distance: $|x_1 - x_2| + |y_1 - y_2|$).
* **$f(n)$:** The total estimated cost of the lowest-cost path passing through node $n$.

[Start Node] ──> Expand Open Set ──> Evaluate Min f(n) ──> Update Closed Set ──> [Goal Reached]


---

## 🛠️ Tech Stack & Architecture

* **Language:** C# (.NET Core / Console)
* **Data Structures:** Custom Priority Lists / Binary Heaps for Open Set evaluation, Hash Sets for Closed Set lookups ($O(1)$ efficiency).
* **Concepts:** Graph Theory, Grid Search, Spatial Navigation, Path Reconstruction Algorithms.

---

## 🚀 Getting Started

### Prerequisites
* [.NET SDK 6.0+](https://dotnet.microsoft.com/download)

### Run the Application

1. **Clone the Repository:**
   ```bash
   git clone [https://github.com/FarukDlmc/A-Star-Pathfinding-Engine.git](https://github.com/FarukDlmc/A-Star-Pathfinding-Engine.git)
