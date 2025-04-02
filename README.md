# BrewServeApi

## Introduction
BrewServer Api is a .Net Core web API managing beers, breweries, and bars.

## Technologies Used
- .Net Core 
- SQL Server
- EF Core
- Serilog (Logging)
- NUnit (Unit Tests)
- FluentValidation

## Api Endpoints

### Beer
- POST /beer - Insert a single beer
- PUT /beer/{id} - Update a beer by Id
- GET /beer?gtAlcoholByVolume=5.0&ltAlcoholByVolume=8.0 - Get all beers with optional filtering query parameters for alcohol content (gtAlcoholByVolume = greater than, ltAlcoholByVolume = less than)
- GET /beer/{id} - Get beer by Id

### Brewery
- POST /brewery - Insert a single brewery
- PUT /brewery/{id} - Update a brewery by Id
- GET /brewery - Get all breweries
- GET /brewery/{id} - Get brewery by Id

### Bar
- POST /bar - Insert a single bar
- PUT /bar/{id} - Update a bar by Id
- GET /bar - Get all bars
- GET /bar/{id} - Get bar by Id

### BeerBar
- POST /bar/beer - Insert a single bar beer link
- GET /bar/{barId}/beer - Get a single bar with associated beers
- GET /bar/beer - Get all bars with associated beers

###  BreweryBeer
- POST /brewery/beer - Insert a single brewery beer link
- GET /brewery/{breweryId}/beer - Get a single brewery by Id with associated beers
- GET /brewery/beer - Get all breweries with associated beers

## Running the Application
1. Clone the repository
2. Update 'appsettings.json' with your SQL Server connection.
3. Update appsettings.json' with your Logging Configuration.
4. Run the API

## Running the Unit Tests

## View logs
Logs are saved in /logs folder by Serilog


