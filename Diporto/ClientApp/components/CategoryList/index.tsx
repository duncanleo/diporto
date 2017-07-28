import * as React from 'react';

interface CategoryListProps extends React.Props<any> {
  categories?: string[]
}

interface CategoryListState extends React.ComponentState {}

export default class CategoryList extends React.Component<CategoryListProps, CategoryListState> {
  public static defaultProps: Partial<CategoryListProps> = {
    categories: []
  }

  private convertToTitleCase(snakeCaseString: string) {
    return snakeCaseString.replace(/_/g, ' ')
  }

  public render() {
    return (
      <div className="pv2">
        {this.props.categories.map((category, index) => {
          let classes = ["mb1", "f7", "lh-copy", "inline-flex", "bg-near-white", "ba", "b--light-gray", "ttu", "light-silver", "br2", "ph2", "ml2"]

          if (index === 0) { classes = classes.filter(string => string !== "ml2") }

          return (
            <span key={category} className={classes.join(' ')}>
              {this.convertToTitleCase(category)}
            </span>
          );
        })}
      </div>
    )
  }

}
