import { getMovies, getStars, Actor, Movie, getHtml } from "./lib/helper";
import * as fs from "fs";

let depth = 10;

const main = async () => {
  let actorQueue: Actor[] = [{ name: "Dana Carvey", link: "/name/nm0001022" }];
  let visitedActors: Actor[] = [];
  let visitedMovies: Movie[] = [];

  const queueActor = (actor: Actor) => {
    if (!data.find((d) => d.name === actor.name)) {
      actorQueue.push(actor);
    }
  };

  let data: { name: string; movies: string[] }[] = [];

  const addData = async (actor: Actor, movies: Movie[]) => {
    let existingActor = data.find((d) => d.name === actor.name);
    if (!existingActor) {
      data = [...data, { name: actor.name, movies: movies.map((m) => m.name) }];
    } else {
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
      let $ = await getHtml(actor.link);
      let movies = await getMovies($);

      await addData(actor, movies);
      visitedActors = [...visitedActors, actor];

      for (let m of movies) {
        if (!visitedMovies.find((mq) => mq.name === m.name)) {
          let $ = await getHtml(m.link);
          for (var s of getStars($)) {
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

export default {};
