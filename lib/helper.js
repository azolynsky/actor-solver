"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.getMovies = exports.getStars = exports.getHtml = void 0;
const axios_1 = __importDefault(require("axios"));
const cheerio_1 = __importDefault(require("cheerio"));
const baseUrl = "https://www.imdb.com";
const movieBreadth = 2;
const getHtml = async (url) => {
    let response = await axios_1.default.get(baseUrl + url);
    let $ = cheerio_1.default.load(response.data);
    return $;
};
exports.getHtml = getHtml;
const getStars = ($) => {
    let bla = $("a:contains('Stars').ipc-metadata-list-item__label")
        .siblings("div")
        .first()
        .find("ul")
        .children("li")
        .toArray();
    return bla.map((el) => {
        return {
            name: $(el).find("a").text(),
            link: $(el).find("a").attr("href"),
        };
    });
};
exports.getStars = getStars;
const getMovies = ($) => {
    let bla = $(".knownfor-title-role").toArray();
    return bla
        .map((el) => {
        return {
            name: $(el).find("a").text(),
            link: $(el).find("a").attr("href"),
        };
    })
        .slice(0, movieBreadth);
};
exports.getMovies = getMovies;
