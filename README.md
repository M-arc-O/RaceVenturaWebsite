# RaceVenturaWebsite
To run localy you need to take the following steps:
1 - add "JwtSecret": "3555e854-b1bd-4bfd-b86d-acdd2ca5b6eb" to the RaceVenturaAPI user secrets. The GUID can be random.
2 - Install docker and run docker run --rm --name pg-docker -e POSTGRES_PASSWORD=docker -d -p 5432:5432 postgres.
3 - Open package Manager Console in VS with project RaceVenturaAPI and run update-database.
4 - (Optional) Import 'RaceVentura Basic Setup.postman_collection' from the PostmanCollections into postman and run for a basic setup.
