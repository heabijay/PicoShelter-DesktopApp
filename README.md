# PicoShelter-WebApp

<p align="center">
    <img src="https://www.picoshelter.tk/assets/icons/picoShelter/Black%20Icon%20%2B%20Text.svg" height="64px">
</p>

The client part (desktop uploader) powered on WPF of the PicoShelter Project.

## About PicoShelter Project

_"PicoShelter is a free image hosting service with user profiles, shared albums, and direct links. Also, we provide the official desktop application for a more comfortable experience."_

PicoShelter's [ApiServer](https://github.com/heabijay/PicoShelter-ApiServer), [WebApp](https://github.com/heabijay/PicoShelter-WebApp), [DesktopApp](https://github.com/heabijay/PicoShelter-DesktopApp) were created by [heabijay](https://github.com/heabijay) as the diploma project.

## Demo

This project demo currently serves on own [WebApp](https://github.com/heabijay/PicoShelter-WebApp) since 05.05.2021.

**Download from WebApp:** [picoshelter.tk/assets/files/PicoShelter-DesktopApp.exe](https://www.picoshelter.tk/assets/files/PicoShelter-DesktopApp.exe)

**Download from WebApp (original url by Azure):** [wonderful-sand-0ac999f03.azurestaticapps.net/assets/files/PicoShelter-DesktopApp.exe](https://wonderful-sand-0ac999f03.azurestaticapps.net/assets/files/PicoShelter-DesktopApp.exe)

_Due to the free plan on Azure App Service (for ApiServer), the server isn't alive all time so the first request could take some time._

## Screenshots
<details>
    <summary>Click to show</summary>

### The first run
![](https://i.imgur.com/L2dwWiv.png)

### Login page
![](https://i.imgur.com/JdUOjHt.png)

### Main page
![](https://i.imgur.com/0L96Taj.png)

### Main page (w/ images)
![](https://i.imgur.com/wNTvMr6.png)

### Settings
![](https://i.imgur.com/x2edDP3.png)
</details>

## Configuration

Configurations that you need, are client and server endpoint in file `PicoShelter-DesktopApp/ServerRouting.cs`.

- `ServerRouting.HomeUrl` — It's the server endpoint.
- `ServerRouting.WebAppRouting` — It's the client endpoint.