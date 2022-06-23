import React from "react";
import { Card, Icon } from "semantic-ui-react";
import { CustomCardInput, CustomCard } from "./custom-cards";

type Linkedin = CustomCardInput & {
  linkedinProfile?: string;
};

export const LinkedinCard = (cardProps: Linkedin) => {
  return (
    <>
      <CustomCard
        cardHeader={cardProps.cardHeader}      
        cardDescription={cardProps.cardDescription}
        cardMeta={cardProps.cardMeta}
        cardMetaType={cardProps.cardMetaType}
        image={cardProps.image}
        link={cardProps.link}

      >
        <Card.Content extra>
          <a href={cardProps.linkedinProfile}>
            <Icon name="linkedin" />
            {cardProps.cardContentExtra}
          </a>
        </Card.Content>
      </CustomCard>
    </>
  );
};
