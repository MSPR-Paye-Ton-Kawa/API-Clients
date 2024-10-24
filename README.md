# Construire l'image Docker
docker build -t api_clients:latest .

# Lister les images Docker
docker images

# Lancer le contenaire
docker run -d -p 8080:80 --name api_clients_container api_clients:latest
