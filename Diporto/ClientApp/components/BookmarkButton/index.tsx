import * as React from 'react';

declare interface Props extends React.Props<any> {
  bookmarked?: boolean
  onClick?: () => void
}

const BookmarkButton: React.SFC<Props> = ({ bookmarked = false, onClick }) => {
  const imageUrl = bookmarked ? "https://s3-us-west-1.amazonaws.com/jurvis/Bookmark.svg" : "https://s3-us-west-1.amazonaws.com/jurvis/BookmarkEmpty.svg"
  const buttonText = bookmarked ? "Bookmarked" : "Bookmark"
  const buttonStyle = {
    border: 0,
    backgroundColor: "#50CCBC"
  }

  return (
    <button onClick={onClick} style={buttonStyle} className="flex items-center pointer white br2 ph2 pv1">
      <img className="h2" src={imageUrl} />
      <span className="f6 lh-copy ml2">{buttonText}</span>
    </button>
  )
}

export default BookmarkButton;