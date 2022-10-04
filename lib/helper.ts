import axios from "axios";
import cheerio, { CheerioAPI } from "cheerio";

const baseUrl = "https://www.imdb.com";

const movieBreadth = 2;

export type Movie = {
  name: string;
  link: string | undefined;
};

export type Actor = {
  name: string;
  link: string | undefined;
};

export const getHtml = async (url: string | undefined) => {
  let response = await axios.get(baseUrl + url);
  let $ = cheerio.load(response.data);
  return $;
};

export const getStars = ($: CheerioAPI): Actor[] => {
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

export const getMovies = ($: CheerioAPI): Movie[] => {
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
