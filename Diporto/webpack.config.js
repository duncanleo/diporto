const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;

const autoprefixer = require('autoprefixer');
const cssnext = require('cssnext');
const precss = require('precss');
const lost = require('lost');
const mqpacker = require('css-mqpacker');

module.exports = (env) => {
  const isDevBuild = !(env && env.prod);

  return {
    devtool: 'source-map',
    entry: [
      './client/app.tsx'
    ],
    resolve: { extensions: ['.js', '.jsx', '.ts', '.tsx'] },
    output: {
      path: path.resolve('dist'),
      publicPath: 'dist',
      filename: 'bundle.js',
    },
    module: {
      rules: [
        { test: /\.(t|j)sx?$/, use: { loader: 'awesome-typescript-loader' } },
        { enforce: "pre", test: /\.js$/, loader: "source-map-loader" },
        {
          test: /\.css$/,
          use: [
            'style-loader',
            { loader: 'typings-for-css-modules-loader', options: { modules: true, namedExport: true } },
            {
              loader: 'postcss-loader',
              options: {
                plugins: function() {
                  return [autoprefixer, precss, cssnext, mqpacker, fontmagician];
                }
              }
            }
          ]
        },
        {
          test: /\.(jpe?g|png|gif)$/i,
          use: [
            'img-loader',
            {
              loader: 'url-loader',
              options: {
                limit: 8192,
              },
            }
          ]
        },
        {
          test: /\.svg$/,
          use: ['raw-loader']
        }
      ]
    },
    resolve: {
      extensions: ['.ts', '.tsx', '.js', '.jsx'],
    }
  }
}