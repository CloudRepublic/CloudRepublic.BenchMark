export const benchMarkService = { getBenchMarkData };

async function getBenchMarkData(cloudProvider, hostingEnvironment, runtime) {
  let response = await fetch("");
  return handleResponse(response);
}

function handleResponse(response) {
  return response.text().then(text => {
    const data = text && JSON.parse(text);
    if (!response.ok) {
      const error = (data && data.message) || response.statusText;
      return Promise.reject(error);
    }

    return data;
  });
}
