import React, { ReactNode } from 'react';
import { Card, Image, CardProps } from 'semantic-ui-react';
import "../custom-cards/custom-cards.css";

export type CustomCardInput = CardProps & {
  cardHeader?: string;
  instructorCardHeader?: string;
  instructorCardHeader2?: string;
  cardDescription?: string;
  image?: string;
  cardMeta?: string;
  cardMetaType?: string;
  cardContentExtra?: string;
  linkedinProfile?: string;
  children?: ReactNode;
  link?: boolean;
  color?: string;
};
export const CustomCard = (cardProps: CustomCardInput) => {
  return (
    <>
      <Card link={cardProps.link} color="blue" onClick={cardProps.onClick}>
        <Image src={cardProps.image} />
        <Card.Content verticalAlign="middle">
          <Card.Header id="cardHeader">{cardProps.cardHeader}</Card.Header>

          <Card.Meta id="cardMeta">
            <span>{cardProps.cardMeta}</span>
          </Card.Meta>
          <Card.Description id="cardDescription">
          {cardProps.cardDescription}
          </Card.Description>

        </Card.Content>
        {cardProps.children}
      </Card>
    </>
  );
};
