import { defineCollection, z } from 'astro:content';
import { glob } from 'astro/loaders';

function stripMd(entry: string) {
  return entry.replace(/\.md$/, '');
}

const categories = defineCollection({
  loader: glob({
    pattern: '**/*.md',
    base: './src/content/categories',
    generateId: ({ entry }) => stripMd(entry),
  }),
  schema: z.object({
    title: z.string(),
    description: z.string(),
    icon: z.string(),
    color: z.string(),
    displayOrder: z.number(),
  }),
});

const projects = defineCollection({
  loader: glob({
    pattern: '**/*.md',
    base: './src/content/projects',
    generateId: ({ entry }) => stripMd(entry),
  }),
  schema: z.object({
    title: z.string(),
    category: z.string(),
    description: z.string(),
    technologies: z.array(z.string()),
    repoPath: z.string(),
    githubUrl: z.string().url(),
    featured: z.boolean().default(false),
    status: z.enum(['active', 'archived', 'learning']).default('active'),
    dateAdded: z.coerce.date(),
    highlights: z.array(z.string()).optional(),
  }),
});

export const collections = { categories, projects };
