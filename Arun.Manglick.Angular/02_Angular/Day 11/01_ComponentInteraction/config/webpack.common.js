var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');

const path = require('path');
const rootDir = path.resolve(__dirname);

module.exports = {
    entry: {
        'polyfills':'./polyfills.ts',
        'vendor':'./vendor.ts',
        'app': './app/main.ts',
    },

    resolve: {
        extensions: ['.js', '.ts']
    },

    module:{
        rules:[
            {
                test:/\.ts$/,
                loaders: [
                    {
                        loader: 'awesome-typescript-loader',
                        options: { configFileName: './tsconfig.json' }
                    }, 'angular2-template-loader'
                ]
            },
            {
                test: /\.html$/,
                loader: 'html-loader'
            }
        ]
    },

    plugins:[
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills']
        }),
        new HtmlWebpackPlugin({
            template: './index.html'
        }),
        new webpack.ContextReplacementPlugin(
            /angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname)
        )
    ]
};