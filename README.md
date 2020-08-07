# dfc-eventstore

## Introduction

DFC Event Store contains an Event Grid Trigger that records all Events via a Subscription to Cosmos DB.

## Local Development

In order to debug locally you need to send a Post request to the following URL:
http://localhost:7071/runtime/webhooks/eventgrid?functionName=Execute

Ensure the following headers are also present - you will receive a 400 response if these are not in place:
- Content-Type = application/json
- aeg-event-type - Notification