import logging

import azure.functions as func


def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    if req.params.get('name'):
        return func.HttpResponse(f"Hello {req.params.get('name')}!")
    else:
        return func.HttpResponse(
            "Please pass a name on the query string",
            status_code=400
        )
