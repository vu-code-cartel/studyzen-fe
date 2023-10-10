import { useTranslation } from 'react-i18next';
import { AppBreadcrumbs } from '../../components/AppBreadcrumbs';
import { PageContainer } from '../../components/PageContainer';
import { PageHeader } from '../../components/PageHeader';
import { usePageCategory } from '../../hooks/usePageCategory';
import { AppRoutes, getFlashcardSetRoute } from '../../common/app-routes';
import { useCreateFlashcard, useGetFlashcards } from '../../hooks/api/useFlashcardsApi';
import { useParams } from 'react-router-dom';
import { getIdFromSlug } from '../../common/utils';
import { CenteredLoader } from '../../components/CenteredLoader';
import { NotFound } from '../../components/NotFound';
import { useGetFlashcardSet } from '../../hooks/api/useFlashcardSetsApi';
import { ActionIcon, Card, Center, Fieldset, Grid, Group, Stack, Tabs, Text, TextInput } from '@mantine/core';
import { IconChevronLeft, IconChevronRight, IconPlus } from '@tabler/icons-react';
import { CreateFlashcardRequest } from '../../api/requests';
import { FlashcardDto, FlashcardSetDto } from '../../api/dtos';
import { useForm } from '@mantine/form';
import { useState } from 'react';
import { FlippableFlashcard } from '../../components/FlippableFlashcard';
import { useAppStore } from '../../hooks/useAppStore';
import { useDocumentTitle } from '@mantine/hooks';

class FlashcardSetTabs {
  public static readonly ViewSet = 'view-set';
  public static readonly Learn = 'learn';
}

export const FlashcardSetPage = () => {
  const { t } = useTranslation();
  const { flashcardSetIdWithSlug } = useParams();
  const { data: flashcardSet, isLoading: isFlashcardSetLoading } = useGetFlashcardSet(
    getIdFromSlug(flashcardSetIdWithSlug),
  );
  const { data: flashcards, isLoading: areFlashcardsLoading } = useGetFlashcards(getIdFromSlug(flashcardSetIdWithSlug));
  usePageCategory('flashcards');
  useDocumentTitle(t('FlashcardSet.DocumentTitle.FlashcardSet', { flashcardSetName: flashcardSet?.name }));

  if (isFlashcardSetLoading || areFlashcardsLoading) {
    return <CenteredLoader />;
  }

  if (!flashcardSet || !flashcards) {
    return <NotFound />;
  }

  return (
    <PageContainer>
      <PageHeader>
        <AppBreadcrumbs
          items={[
            { title: t('FlashcardSet.Title.Flashcards'), to: AppRoutes.FlashcardSets },
            { title: flashcardSet.name, to: getFlashcardSetRoute(flashcardSet.id, flashcardSet.name) },
          ]}
        />
      </PageHeader>

      <Tabs color='teal' defaultValue={FlashcardSetTabs.ViewSet} keepMounted={false}>
        <Tabs.List mb='md'>
          <Tabs.Tab value={FlashcardSetTabs.ViewSet}>{t('FlashcardSet.Tab.ViewSet')}</Tabs.Tab>
          <Tabs.Tab value={FlashcardSetTabs.Learn}>{t('FlashcardSet.Tab.Learn')}</Tabs.Tab>
        </Tabs.List>

        <Tabs.Panel value={FlashcardSetTabs.ViewSet}>
          <ViewSetPanel flashcardSet={flashcardSet} flashcards={flashcards} />
        </Tabs.Panel>
        <Tabs.Panel value={FlashcardSetTabs.Learn}>
          <LearnPanel flashcardSet={flashcardSet} flashcards={flashcards} />
        </Tabs.Panel>
      </Tabs>
    </PageContainer>
  );
};

interface FlashcardSetPanelProps {
  flashcardSet: FlashcardSetDto;
  flashcards: FlashcardDto[];
}

const ViewSetPanel = (props: FlashcardSetPanelProps) => {
  const createFlashcard = useCreateFlashcard();
  const isMobile = useAppStore((state) => state.isMobile);
  const { t } = useTranslation();
  const form = useForm<CreateFlashcardRequest>({
    initialValues: {
      flashcardSetId: 0,
      front: '',
      back: '',
    },
    validate: {},
  });

  const onCreateFlashcardClick = async (request: CreateFlashcardRequest, flashcardSet: FlashcardSetDto) => {
    request.flashcardSetId = flashcardSet.id;
    await createFlashcard.mutateAsync(request);
  };

  return (
    <Stack>
      <Text fw={600}>{t('FlashcardSet.TermsInSet')}</Text>
      <form
        onSubmit={form.onSubmit((values) => onCreateFlashcardClick(values, props.flashcardSet))}
        autoComplete='off'
        spellCheck='false'
      >
        <Fieldset m={0} p='md'>
          <Stack gap='sm'>
            <Grid>
              <Grid.Col span={isMobile ? 12 : 6}>
                <TextInput label={t('Flashcard.Field.Front')} withAsterisk {...form.getInputProps('front')} />
              </Grid.Col>
              <Grid.Col span={isMobile ? 12 : 6}>
                <TextInput label={t('Flashcard.Field.Back')} withAsterisk {...form.getInputProps('back')} />
              </Grid.Col>
            </Grid>

            <Group justify='end'>
              <Group gap='xs'>
                <ActionIcon variant='light' color='teal' type='submit'>
                  <IconPlus stroke={1.5} width='70%' height='70%' />
                </ActionIcon>
              </Group>
            </Group>
          </Stack>
        </Fieldset>
      </form>

      {props.flashcards.map((flashcard) => (
        <Card withBorder key={flashcard.id}>
          <Grid>
            <Grid.Col span={6}>
              <Text>{flashcard.front}</Text>
            </Grid.Col>
            <Grid.Col span={6}>
              <Text>{flashcard.back}</Text>
            </Grid.Col>
          </Grid>
        </Card>
      ))}
    </Stack>
  );
};

const LearnPanel = (props: FlashcardSetPanelProps) => {
  const [activeFlashcardIdx, setActiveFlashcardIdx] = useState<number | null>(props.flashcards.length > 0 ? 0 : null);
  const { t } = useTranslation();

  if (activeFlashcardIdx === null) {
    return <Text>{t('FlashcardSet.SetDoesNotContainAnyFlashcards')}</Text>;
  }

  return (
    <Stack>
      <FlippableFlashcard
        front={props.flashcards[activeFlashcardIdx].front}
        back={props.flashcards[activeFlashcardIdx].back}
        color={props.flashcardSet.color}
      />

      <Center>
        <Group>
          <ActionIcon
            size='lg'
            variant='default'
            disabled={activeFlashcardIdx <= 0}
            onClick={() => setActiveFlashcardIdx((prev) => prev! - 1)}
          >
            <IconChevronLeft stroke={1.5} />
          </ActionIcon>
          <Text fw={500}>
            {activeFlashcardIdx + 1} / {props.flashcards.length}
          </Text>
          <ActionIcon
            size='lg'
            variant='default'
            disabled={activeFlashcardIdx >= props.flashcards.length - 1}
            onClick={() => setActiveFlashcardIdx((prev) => prev! + 1)}
          >
            <IconChevronRight stroke={1.5} />
          </ActionIcon>
        </Group>
      </Center>
    </Stack>
  );
};