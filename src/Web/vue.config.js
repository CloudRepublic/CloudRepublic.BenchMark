const webpack = require('webpack');
const isProd = process.env.NODE_ENV === 'production';

module.exports = {
  publicPath: isProd ? '' : '',
  configureWebpack: {
    // Set up all the aliases we use in our app.
    devtool: 'source-map',
    resolve: {
      extensions: ['.wasm', '.mjs', '.js', '.jsx', '.json']
    },
    module: {
      rules: [
        {
          test: /\.mjs$/,
          include: /node_modules/,
          type: 'javascript/auto'
        }
      ]
    },
    plugins: [
      new webpack.optimize.LimitChunkCountPlugin({
        maxChunks: 6
      })
    ]
  },
  pwa: {
    name: 'Serverless Benchmark',
    themeColor: '#172b4d',
    msTileColor: '#172b4d',
    appleMobileWebAppCapable: 'yes',
    appleMobileWebAppStatusBarStyle: '#172b4d'
  },
  css: {
    // Enable CSS source maps.
    sourceMap: process.env.NODE_ENV !== 'production'
  }
};
