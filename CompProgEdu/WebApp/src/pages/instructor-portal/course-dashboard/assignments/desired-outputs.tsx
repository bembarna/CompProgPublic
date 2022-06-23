import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import { useAsync } from 'react-use';
import { Header, Button, Icon, Table, Ref } from 'semantic-ui-react';
import {
  DesiredOutputGetDto,
  DesiredOutputsService,
} from '../../../../swagger';
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import {
  DesiredOutputCreateModal,
  DesiredOutputUpdateModal,
} from './desired-output-create-update-modal';
import { useToasts } from 'react-toast-notifications';

type DesiredOutputs = {
  refetchAssignment: any;
};

export const DesiredOutputs = (props: DesiredOutputs) => {
  const [createModal, setCreateModal] = useState<boolean>(false);
  const [updateModal, setUpdateModal] = useState<boolean>(false);
  const [selectedOutputId, setSelectedOutputId] = useState<number>();
  const [loadingSubmitOrder, setLoadingSubmitOrder] = useState<boolean>(false);
  const [outputs, setOutputs] = useState<DesiredOutputGetDto[]>();
  const [editingOrder, setEditingOrder] = useState<boolean>(false);
  const { assignmentId } = useParams() as any;
  const { addToast } = useToasts();

  const fetchDesiredOutputs = useAsync(async () => {
    const response = await DesiredOutputsService.getByAssignmentId({
      assignmentId: assignmentId,
    });
    setOutputs(response.result);
    if (props.refetchAssignment) {
      props.refetchAssignment();
    }
    return response.result;
  }, [createModal, updateModal, editingOrder]);

  let desiredOutputs = fetchDesiredOutputs.value;

  const reorder = (list: any, startIndex: any, endIndex: any) => {
    const result = Array.from(list);
    const [removed] = result.splice(startIndex, 1);
    result.splice(endIndex, 0, removed);

    return result;
  };

  const getItemStyle = (isDragging: any, draggableStyle: any) => ({
    background: isDragging && 'lightblue',
    display: isDragging && 'table',
    ...draggableStyle,
  });

  const onDragEnd = (result: any) => {
    if (!result.destination) {
      return;
    }

    const items = reorder(
      outputs,
      result.source.index,
      result.destination.index
    ) as DesiredOutputGetDto[];

    items.forEach((x, index) => {
      x.order = index;
    });

    setOutputs(items);
  };

  const submitOrder = async () => {
    setLoadingSubmitOrder(true);
    const response = await DesiredOutputsService.updateOrder({ body: outputs });
    try {
      if (response.errors.length > 0) {
        addToast('Error while updating order.', { appearance: 'error' });
        setLoadingSubmitOrder(false);
        setEditingOrder(false);
      } else {
        addToast('Successfully Updated Order!', {
          appearance: 'success',
        });
        setLoadingSubmitOrder(false);
        setEditingOrder(false);
      }
    } catch {
      addToast('Error! Something went wrong', { appearance: 'error' });
      setLoadingSubmitOrder(false);
      setEditingOrder(false);
    }
  };

  return (
    <>
      <div style={{ marginTop: 20 }}>
        <div style={{ display: 'flex', flexDirection: 'row' }}>
          <Header as="h1">Desired Outputs</Header>
          {editingOrder ? (
            <Button
              style={{ marginLeft: 'auto', height: 35 }}
              color="green"
              onClick={() => submitOrder()}
              loading={loadingSubmitOrder}
            >
              <Icon name="check" />
              Save Changes
            </Button>
          ) : (
            <>
              <Button
                style={{ marginLeft: 'auto', height: 35 }}
                primary
                disabled={desiredOutputs && desiredOutputs.length <= 1}
                onClick={() => setEditingOrder(true)}
              >
                <Icon name="edit" />
                Edit Order
              </Button>

              <Button
                style={{ height: 35 }}
                color="black"
                onClick={() => setCreateModal(true)}
              >
                <Icon name="plus" />
                Add Desired Output
              </Button>
            </>
          )}
        </div>
        {editingOrder && (
          <div>
            Drag and drop your desired outputs in the order you would like them
            to be checked.
          </div>
        )}
        <Table>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell width="6">Input</Table.HeaderCell>
              <Table.HeaderCell width="6">Output</Table.HeaderCell>
              <Table.HeaderCell width="3">Points</Table.HeaderCell>
              <Table.HeaderCell width="2" />
            </Table.Row>
          </Table.Header>

          {desiredOutputs?.length !== 0 ? (
            <DragDropContext onDragEnd={onDragEnd}>
              <Droppable droppableId="droppable">
                {(provided) => (
                  <Ref innerRef={provided.innerRef}>
                    <Table.Body {...provided.droppableProps}>
                      {outputs?.map((x: DesiredOutputGetDto, index: any) => {
                        const key = `${x.id}`;
                        return (
                          <>
                            <React.Fragment key={key}>
                              <Draggable
                                key={key}
                                draggableId={key}
                                index={index}
                                isDragDisabled={!editingOrder}
                              >
                                {(provided, snapshot) => (
                                  <Ref innerRef={provided.innerRef}>
                                    <Table.Row
                                      {...provided.draggableProps}
                                      {...provided.dragHandleProps}
                                      style={getItemStyle(
                                        snapshot.isDragging,
                                        provided.draggableProps.style
                                      )}
                                      ref={provided.innerRef}
                                      isDragging={snapshot.isDragging}
                                    >
                                      <Table.Cell width="6">
                                        {x.input}
                                      </Table.Cell>
                                      <Table.Cell width="6">
                                        {x.output}
                                      </Table.Cell>
                                      <Table.Cell width="3">
                                        {x.pointValue}
                                      </Table.Cell>
                                      <Table.Cell width="2">
                                        <Button
                                          color="black"
                                          disabled={editingOrder}
                                          onClick={() => {
                                            setSelectedOutputId(x.id);
                                            setUpdateModal(true);
                                          }}
                                        >
                                          Edit
                                        </Button>
                                      </Table.Cell>
                                    </Table.Row>
                                  </Ref>
                                )}
                              </Draggable>
                            </React.Fragment>
                          </>
                        );
                      })}
                      {provided.placeholder}
                    </Table.Body>
                  </Ref>
                )}
              </Droppable>
            </DragDropContext>
          ) : (
            <Table.Body>
              <Table.Row>
                <Table.Cell colSpan="4">
                  <Header>You Currently Have No Desired Outputs</Header>
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          )}
        </Table>
      </div>
      <DesiredOutputCreateModal
        assignmentId={assignmentId}
        count={Number(desiredOutputs?.length)}
        modalState={createModal}
        setModalState={(value: boolean) => setCreateModal(value)}
      />
      <DesiredOutputUpdateModal
        outputId={selectedOutputId}
        modalState={updateModal}
        setModalState={(value: boolean) => setUpdateModal(value)}
      />
    </>
  );
};
