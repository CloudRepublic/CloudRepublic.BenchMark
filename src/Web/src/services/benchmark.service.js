export const benchMarkService = { getBenchMarkData };

async function getBenchMarkData(cloudProvider, hostingEnvironment, runtime) {
  let response = await fetch(
    `${
    process.env.VUE_APP_API
    }?cloudProvider=${cloudProvider}&hostingEnvironment=${hostingEnvironment}&runtime=${runtime}`,
    {
      headers: { 'Ocp-Apim-Subscription-Key': process.env.VUE_APP_API_KEY }
    }
  );
  return handleResponse(response);
}

function handleResponse(response) {

  if (!response.ok || response.status != 200) {
    return null;
  }

  return response.text().then(text => {
    const data = text && JSON.parse(text);
    return data;
  });
}
