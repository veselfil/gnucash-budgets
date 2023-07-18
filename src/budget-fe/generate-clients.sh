URL="http://localhost:5192/swagger/v1/swagger.json"

swagger_file="swagger.json"

http get "${URL}" > "${swagger_file}"

