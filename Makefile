build:
	docker build -t server .
run:
	docker run --rm -p 5000:5040 server
