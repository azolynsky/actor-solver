import cheerio from 'cheerio';
import axios from 'axios';

const main = async() =>{
    const resp = await axios.get('http://www.reddit.com/r/programming.json');
    console.log(resp.data);
    
    // const $ = cheerio.load('<h2 class="title">Hello world</h2>');
    
    // $('h2.title').text('Hello there!')
    // $('h2').addClass('welcome')
    
    // $.html()
}

main();

export default {};