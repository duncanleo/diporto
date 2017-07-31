# Diporto [![Build Status](https://travis-ci.com/duncanleo/diporto.svg?token=K55cj8GL5QGanosi8wGd&branch=master)](https://travis-ci.com/duncanleo/diporto) [![VSTS Build Status](https://diporto.visualstudio.com/_apis/public/build/definitions/1eb9e35c-ebb2-4df6-bca3-70b4a9d7b9c4/3/badge)](https://diporto.visualstudio.com/Diporto/_build/index?context=mine&path=%5C&definitionId=3&_a=completed)

<img src="https://user-images.githubusercontent.com/5944973/28773399-fc7ea2da-761b-11e7-94bc-e9aa7ab8d8b7.png" alt="Diporto Icon" align="right" />
Diporto is a Foursquare clone for finding the best recommendations for places near you in Singapore. A web application developed for a school assignment.

#### Features
- Search for places
- Bookmark places for later
- Leave Reviews for places you've visited

#### Should I Use This?
Probably not, I like [Foursquare](https://foursquare.com/) and [Yelp](https://yelp.com).

#### Things you need to have installed
1. .NET Core
2. Node
3. Yarn

#### Getting Up and Running
1. Clone this repository
2. Navigate to Diporto/.vscode/launch.json and fill up the keys required for S3 and the Google Images API: `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `GOOGLE_API_KEY`
3. Download JS dependencies by navigating to the `Diporto/` directory and running `yarn`
4. Run `yarn setup`
5. Open this project up in Visual Studio 2017/Visual Studio Code
6. Hit F5 and the website should then be hosted locally on `localhost:5000`
