const path = require('path'); 
const { CleanWebpackPlugin } = require('clean-webpack-plugin'); 
const MiniCssExtractPlugin = require('mini-css-extract-plugin'); 
const TerserPlugin = require('terser-webpack-plugin'); 
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin'); 
module.exports = { 
    entry: { 
        main: './wwwroot/js/site.js', 
        vendor: './wwwroot/js/vendor.js' 
    }, 
    output: { 
        filename: '[name].bundle.js', 
        path: path.resolve(__dirname, 'wwwroot/dist'), 
        publicPath: '/dist/' 
    }, 
    module: { 
        rules: [ 
            { 
                test: /\.css$/, 
                use: [MiniCssExtractPlugin.loader, 'css-loader'] 
            }, 
            { 
                test: /\.(png|jpg|gif|svg)$/, 
                type: 'asset/resource', 
                generator: { 
                    filename: 'images/[hash][ext][query]' 
                } 
            } 
        ] 
    }, 
    plugins: [ 
        new CleanWebpackPlugin(), 
        new MiniCssExtractPlugin({ 
            filename: '[name].css' 
        }) 
    ], 
    optimization: { 
        minimize: true, 
        minimizer: [ 
            new TerserPlugin({ 
                parallel: true, 
                terserOptions: { 
                    compress: { 
                        drop_console: true 
                    } 
                } 
            }), 
            new CssMinimizerPlugin() 
        ], 
        splitChunks: { 
            chunks: 'all' 
        } 
    }, 
    devtool: 'source-map', 
    mode: 'production' 
}; 