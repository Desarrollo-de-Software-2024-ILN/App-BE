import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateSerieDto {
  title?: string;
  genre?: string;
  descripcion?: string;
}

export interface SerieDto extends EntityDto<number> {
  title?: string;
  genre?: string;
  descripcion?: string;
}
