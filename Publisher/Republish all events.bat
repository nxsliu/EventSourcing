::curl -v -X DELETE http://127.0.0.1:2113/streams/applications

curl -i -d @event.txt "http://127.0.0.1:2113/streams/applications" -H "Content-Type:application/vnd.eventstore.events+json"
pause