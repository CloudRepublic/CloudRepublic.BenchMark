export const benchMarkService = { getBenchMarkData, getCategories };

async function getBenchMarkData(cloudProvider, hostingEnvironment, runtime, language, sku) {
  let response = await fetch(
    `/api/statistics?cloudProvider=${cloudProvider}&hostingEnvironment=${hostingEnvironment}&runtime=${runtime}&language=${language}&sku=${sku}`
  );
  return handleResponse(response);
}

async function getCategories() {
  let response = await fetch(
    `/api/categories`
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
