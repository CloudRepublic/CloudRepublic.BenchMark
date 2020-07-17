module.exports = async function(context, req) {
  context.log("JavaScript HTTP trigger function processed a request.");

  if (req.query.name) {
    context.res = {
      body: "Hello " + req.query.name
    };
  } else {
    context.res = {
      status: 400,
      body: "Please pass a name on the query string"
    };
  }
};
