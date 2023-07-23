URL="http://localhost:5192/swagger/v1/swagger.json"

swagger_file="swagger.json"
http get "${URL}" > "${swagger_file}"

rm -rf src/_backup-gc-client
mv src/gc-client src/_backup-gc-client

openapi --input "${swagger_file}" --output ./src/gc-client