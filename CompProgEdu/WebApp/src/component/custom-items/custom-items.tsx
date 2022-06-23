import React from "react";
import { Item, Image, Header } from "semantic-ui-react";
import "./custom-items.css";

type CustomItemInput = {
  itemHeader: string;
  itemDescription: string;
  leftImage?: string;
  rightImage?: string;
  itemMeta?: string;
  itemMetaType?: string;
};
export const CustomItem = (itemProps: CustomItemInput) => {
  return (
    <>
      <Item>
        <Item.Image size="small" src={itemProps.leftImage} />
        <Item.Content verticalAlign="middle">
          <Item.Header id="itemHeader">{itemProps.itemHeader}</Item.Header>

          <Item.Description id="itemDescription">
            {itemProps.itemDescription}
          </Item.Description>
        </Item.Content>
        <Item.Image size="small" src={itemProps.rightImage} />
      </Item>
    </>
  );
};
