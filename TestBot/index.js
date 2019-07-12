'use strict';

const Hapi = require('hapi');
//const server = new Hapi.Server();
const server = new Hapi.Server({  
  host: 'localhost',
  port: 3000
})
//server.connection({
 // port: process.env.PORT || 8000
//});

server.route({
  method: 'POST',
  path: '/webhook',
  handler: require('./handlers').answer,
  config: {
    validate: {
      payload: require('./bot/validation').webhookValidationSchema()
    }
  }
});

server.start((err) => {
  if (err) throw err;
  console.log('Server running at:', server.info.uri);
});