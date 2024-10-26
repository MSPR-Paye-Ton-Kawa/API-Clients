# PS-2024-MSPR-Paye-Ton-Kawa

## 📚 Projet Scolaire | MSPR

Juin-Septembre 2024

Groupe : Juliette, Flavien, Yasmine & Colas

### 📌 Consignes du projet : 

CERTIFICATION PROFESSIONNELLE EXPERT EN INFORMATIQUE ET SYSTEME D’INFORMATION

BLOC 4 – Concevoir et développer des solutions applicatives métier et spécifiques (mobiles, embarquées et ERP)

Cahier des Charges de la MSPR « Conception d’une solution applicative en adéquation avec l’environnement technique étudié


### 🐱 Notre projet :

Ce repos est destiné à l'API Clients.

📦 Table Customers :

- CustomerId (int, Primary Key, Auto-increment) : Identifiant unique du client.

- LastName (varchar, not null) : Nom de famille du client.

- FirstName (varchar, not null) : Prénom du client.

- Email (varchar, not null) : Adresse email du client.

- Address (varchar, not null) : Adresse du client.

- PhoneNumber (varchar, not null) : Numéro de téléphone du client.


Commandes Docker :

docker build -t api_clients:latest .

docker run -d -p 8080:80 --name api_clients_container api_clients:latest


### 📎 Branches :

- main : Solution finale, prod.
  
- dev : Solution fonctionnelle en dev.
  
- hotfix : Correction de bugs et autres.

- release : Solution fonctionnelle de dev à prod.

- feature-db : Développement lié à la base de données.

- feature-tests : Développement des tests.

- feature-messagebroker : Développement de la partie message broker.

- feature-docker : Développement de la partie Docker.

- bugfix-* : Correction de bugs.


### 💻 Applications et langages utilisés :

- C#
- Visual Studio
- Docker



## 🌸 Merci !
© J-IFT
