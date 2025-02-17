# Pokémon Look-Up 🌍🔍  
*A Unity-based mobile game integrating real-world weather/location data with Pokémon catching mechanics, powered by PlayFab backend services.*  

[![Unity Version](https://img.shields.io/badge/Unity-2022.3+-black?logo=unity)](https://unity.com)  
[![PlayFab](https://img.shields.io/badge/Backend-PlayFab-blue)](https://playfab.com)  
[![OpenWeather](https://img.shields.io/badge/API-OpenWeather-%2369B6D2)](https://openweathermap.org)  

---

## 🌟 Key Features  
- **Real-World Integration**  
  - Location-based Pokémon spawning using GPS/geolocation (`LocationServiceController.cs`).  
  - Dynamic weather effects tied to OpenWeather API data (`WeatherController.cs`).  
- **PlayFab Backend**  
  - User authentication & account management (`PlayFabManager.cs`).  
  - Leaderboards (`PlayFabLeaderboard.cs`) & virtual currency system (`PlayFabVirtualCurrency.cs`).  
  - Pokémon catch tracking & persistence (`CatchPokemon.cs`).  
- **Mobile-First Design**  
  - Optimized touch controls for catching mechanics.  

---

## 🛠️ Technical Implementation  
### Backend Services  
| Script                   | Functionality                              |  
|--------------------------|--------------------------------------------|  
| `PlayFabManager.cs`      | Handles user login, data storage, and API setup. |  
| `PlayFabLeaderboard.cs`  | Manages global/regional leaderboards.      |  
| `PlayFabVirtualCurrency.cs` | Tracks in-game currency (e.g., PokéCoins). |  

### Gameplay Systems  
| Script                       | Role                                      |  
|------------------------------|-------------------------------------------|  
| `LocationServiceController.cs` | Fetches GPS data for real-world mapping. |  
| `WeatherController.cs`       | Pulls OpenWeather API for local weather. |  
| `CatchPokemon.cs`            | Implements catching minigame logic.      |  

### Tech Stack  
- **Engine**: Unity 2022 LTS  
- **Backend**: PlayFab SDK (User Data, Cloud Scripts, Leaderboards)  
- **APIs**: OpenWeather Map (Real-time weather conditions)  
- **Language**: C#  

---

## 🚀 Getting Started  
### Installation  
1. **Clone the repository**:  
   ```bash
   git clone https://github.com/comedianhhh/Pokemon-Look-Up.git


## 📸 Media
Feature	Screenshot/Video
Catching Mechanic	Catching Demo
Leaderboard	Leaderboard
Weather Integration	Weather System
## 🔍 Why This Project?
Learning Goals:

Integration of multiple APIs (PlayFab + OpenWeather).

Mobile-first design with GPS/weather mechanics.

Scalable backend architecture for live-ops.

## Technical Challenges:

Syncing real-world weather to in-game spawn rates.

Optimizing location services for battery efficiency.
   
