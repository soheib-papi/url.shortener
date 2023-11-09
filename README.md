# URL Shortener

## Description:
A program that takes internet addresses and returns their shortened versions.

## This program consists of four Minimal APIs:

https://localhost:7086/urlitems/?url=https://www.soheib111.ir/
This endpoint receives a URL as a query string and returns its shortened version. The calling method is POST.

https://localhost:7086/urlitems/{id}
This endpoint takes an ID through the address and returns its original address. The calling method is GET.

https://localhost:7086/urlitems/visits/{id}
This endpoint takes an ID and returns the number of visits to this address. The calling method is GET.

https://localhost:7086/urlitems/summary
This endpoint displays all links that have not expired, along with the number of visits. The calling method is GET.

You can use the http file available in the project to test the endpoints.

## Features:
## Microsoft Entity Framework Core
## Minimal API
## Global Exception Handling
## Rate Limiting
## Option Pattern
