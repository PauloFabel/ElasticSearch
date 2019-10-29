# ElasticSearch
Processo de exemplo do funcionamento do elastic
## Passos de instalação o elastic no docker
1. Baixar o elastic. 

    docker  pull  docker.elastic.co/elasticsearch/elasticsearch:6.3.0
2. Baixar o kibana.
	
	docker  pull  docker.elastic.co/kibana/kibana:6.3.0
3. Cria a ponte para a comunicação do elastic com o kibana.
	
	docker network create esnetwork --driver=bridge
4. Inicializa o elastic.

	docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" --name elasticsearch -d --network esnetwork docker.elastic.co/elasticsearch/elasticsearch:6.3.0
5. Inicializa o kibana.

	docker run -p 5601:5601 --name kibana -d --network esnetwork docker.elastic.co/kibana/kibana:6.3.0

6. Roda o compose do yaml.

	docker-compose up
