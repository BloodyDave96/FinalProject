#!/bin/bash

BASE_URL="http://localhost:5000/api"

# Test GET all customers
echo "GET all customers:"
curl -X GET "$BASE_URL/customers" -H "accept: application/json"
echo -e "\n"

# Test POST new customer
echo "POST new customer:"
curl -X POST "$BASE_URL/customers" -H "accept: application/json" -H "Content-Type: application/json" -d '{"name":"John Doe"}'
echo -e "\n"

# Test GET customer by ID
echo "GET customer by ID:"
curl -X GET "$BASE_URL/customers/1" -H "accept: application/json"
echo -e "\n"

# Test PUT update customer
echo "PUT update customer:"
curl -X PUT "$BASE_URL/customers/1" -H "accept: application/json" -H "Content-Type: application/json" -d '{"id":1,"name":"Jane Doe"}'
echo -e "\n"

# Test DELETE customer
echo "DELETE customer:"
curl -X DELETE "$BASE_URL/customers/1" -H "accept: application/json"
echo -e "\n"

# Test gRPC order request
echo "Testing gRPC order request:"
grpcurl -d '{"id": 1}' -plaintext localhost:5001 orders.OrderService/GetOrder
echo -e "\n"
