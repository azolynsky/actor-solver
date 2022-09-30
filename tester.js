"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const axios_1 = __importDefault(require("axios"));
const main = async () => {
    const resp = await axios_1.default.get('http://www.reddit.com/r/programming.json');
    console.log(resp.data);
    // const $ = cheerio.load('<h2 class="title">Hello world</h2>');
    // $('h2.title').text('Hello there!')
    // $('h2').addClass('welcome')
    // $.html()
};
main();
exports.default = {};
