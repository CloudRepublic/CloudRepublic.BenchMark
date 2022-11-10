export const benchMarkService = { getBenchMarkData };

async function getBenchMarkData(cloudProvider, hostingEnvironment, runtime) {
  let response = await fetch(
    `/api/statistics?cloudProvider=${cloudProvider}&hostingEnvironment=${hostingEnvironment}&runtime=${runtime}`
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
