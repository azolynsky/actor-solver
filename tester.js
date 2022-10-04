"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
const helper_1 = require("./lib/helper");
const fs = __importStar(require("fs"));
let depth = 10;
const main = async () => {
    let actorQueue = [{ name: "Dana Carvey", link: "/name/nm0001022" }];
    let visitedActors = [];
    let visitedMovies = [];
    const queueActor = (actor) => {
        if (!data.find((d) => d.name === actor.name)) {
            actorQueue.push(actor);
        }
    };
    let data = [];
    const addData = async (actor, movies) => {
        let existingActor = data.find((d) => d.name === actor.name);
        if (!existingActor) {
            data = [...data, { name: actor.name, movies: movies.map((m) => m.name) }];
        }
        else {
            existingActor.movies = [
                ...existingActor.movies,
                ...movies
                    .map((m) => m.name)
                    .filter((m) => !existingActor?.movies.includes(m)),
            ];
        }
    };
    while (depth > 0) {
        console.log(depth + " iterations remaining");
        depth--;
        let actor = actorQueue.pop();
        if (actor && !visitedActors.find((a) => a.name === actor?.name)) {
            let $ = await (0, helper_1.getHtml)(actor.link);
            let movies = await (0, helper_1.getMovies)($);
            await addData(actor, movies);
            visitedActors = [...visitedActors, actor];
            for (let m of movies) {
                if (!visitedMovies.find((mq) => mq.name === m.name)) {
                    let $ = await (0, helper_1.getHtml)(m.link);
                    for (var s of (0, helper_1.getStars)($)) {
                        queueActor(s);
                        addData(s, [m]);
                        visitedMovies.push(m);
                    }
                }
            }
        }
    }
    console.log(data);
    fs.writeFile("./core-data.json", JSON.stringify(data), function (err) {
        if (err) {
            return console.log(err);
        }
        console.log("Data updated.");
    });
};
main();
exports.default = {};
