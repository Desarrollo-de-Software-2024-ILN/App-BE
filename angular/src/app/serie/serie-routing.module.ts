import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { SeriesComponent } from './series/series.component';

const routes: Routes = [
    {
    path: '', component: SeriesComponent
    
    },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SerieRoutingModule {}