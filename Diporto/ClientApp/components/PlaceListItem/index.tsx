import * as React from 'react';
import { Link } from 'react-router-dom';
import CategoryList from '../CategoryList';

interface PlaceListItemProps {
  place?: Place
  index: number
}

const PlaceListItem: React.SFC<PlaceListItemProps> = (props) => {
  const { place } = props;

  const placePhotoUrl = place.photos[0] === undefined ?  'http://via.placeholder.com/100x100' : `/api/photos/${place.photos[0].id}`

  const photoStyle = {
    "background": `url(${placePhotoUrl}) center`
  }

  return (
    <div className="flex">
      <div className="flex w-60">
	<div className="w-20 pa2" style={{flexShrink: 0}}>
	  <div className="aspect-ratio aspect-ratio--1x1">
	    <div className="aspect-ratio--object cover" style={photoStyle}></div>
	  </div>
	</div>
	<div className="flex flex-column">
	  <Link className="link blue lh-title" to={`/place?id=${place.id}`}>
	    <span className="fw7 lh-title underline-hover">{props.index + 1}. {place.name}</span>
	  </Link>
	  <CategoryList categories={place.categories}/>
	  {place.reviews[0] != null ? <span className="f6 i lh-copy">"{place.reviews[0].text}"</span> : ''}
	</div>
      </div>
      <div className="w-40">
	<span>{place.phone}</span>
	<address>{place.address}</address>
      </div>
    </div>
  )
}

export default PlaceListItem;