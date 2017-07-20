import * as React from 'react';

interface PlaceListItemProps {
  place?: Place
}

const PlaceListItem: React.SFC<PlaceListItemProps> = (props) => {
  const { place } = props;
  return (
    <table>
      <tr>{place.id}</tr>
      <tr>{place.name}</tr>
      <tr>{place.address}</tr>
      <tr>{place.phone}</tr>
    </table>
  )
}

export default PlaceListItem;