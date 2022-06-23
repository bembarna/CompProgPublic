const https = require('https');
const fs = require('fs');
const { codegen } = require(`swagger-axios-codegen`);
const axios = require('axios');

async function useSwagger() {
  const paths = {
    output: './src/swagger',
    remoteUrl: 'https://localhost:5001/swagger/v1/swagger.json',
  };

  const json = `${paths.output}/swagger.json`;

  const { data } = await axios.get(paths.remoteUrl, {
    httpsAgent: new https.Agent({
      rejectUnauthorized: false,
    }),
  });

  fs.writeFileSync(json, JSON.stringify(data, null, 2));

  codegen({
    methodNameMode: 'operationId',
    outputDir: paths.output,
    source: require(json),
    useStaticMethod: true,
  });
}

(async () => {
  await useSwagger();
})();
